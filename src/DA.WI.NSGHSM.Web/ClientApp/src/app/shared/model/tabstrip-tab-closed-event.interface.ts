export interface TabstripTabClosedEvent {
    /** @summary index of the closing tab */
    index: number;
    /** @summary present if the master list for "entityName" should be refreshed asap */
    entityName?: string;
}
